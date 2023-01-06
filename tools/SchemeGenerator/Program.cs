using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;

namespace SchemeGenerator
{
    internal class Program
    {
        static  void Main(string[] args)
        {
            string json = File.ReadAllText("Employee.json");
            var schemaFromFile = JsonSchema.FromSampleJson(json);
            var sc =  JsonSchema.FromUrlAsync("https://core.telegram.org/schema/json");
            sc.Wait();
            var res = sc.Result;
            var classGenerator = new CSharpGenerator(schemaFromFile, new CSharpGeneratorSettings
            {
                ClassStyle = CSharpClassStyle.Record,
                Namespace = "MissBot",
                GenerateJsonMethods = true,
                GenerateNativeRecords = true, GenerateDataAnnotations = false, SchemaType = SchemaType.OpenApi3                
            });;
            var codeFile = classGenerator.GenerateFile();
            File.WriteAllText("Employee1.cs", codeFile);
            Console.Write(codeFile);
            Console.ReadKey();
        }
    }
}
