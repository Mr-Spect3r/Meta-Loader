using dnlib.DotNet;
using System;
using System.IO;
using System.Linq;

namespace Builder.Modules
{
    internal class Obfuscator
    {
        public class RenameProtector
        {
            public static int count_xxx = 0;

            public static void Execute(ModuleDef module)
            {
                module.Name = "M3taaaa";

                foreach (TypeDef type in module.Types)
                {
                    if (type.IsGlobalModuleType || type.IsRuntimeSpecialName || type.IsSpecialName || type.IsWindowsRuntime || type.IsInterface)
                        continue;

                    count_xxx++;

                    type.Name = RandomString(40);
                    type.Namespace = "";

                    foreach (PropertyDef property in type.Properties)
                    {
                        count_xxx++;
                        property.Name = RandomString(40);
                    }

                    foreach (FieldDef fields in type.Fields)
                    {
                        count_xxx++;
                        fields.Name = RandomString(40);
                    }

                    foreach (EventDef eventdef in type.Events)
                    {
                        count_xxx++;
                        eventdef.Name = RandomString(40);
                    }

                    foreach (MethodDef method in type.Methods)
                    {
                        if (method.IsConstructor) continue;
                        count_xxx++;
                        method.Name = RandomString(40);
                    }
                }
            }

            private static Random random = new Random();

            private static string RandomString(int length)
            {
                const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                return new string(Enumerable.Repeat(chars, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            }
        }
    }
}
