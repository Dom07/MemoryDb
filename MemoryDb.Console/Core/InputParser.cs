using MemoryDb.Console.Models;
using Newtonsoft.Json;

namespace MemoryDb.Console.Core
{
    public class InputParser
    {
        public Command Parse(string input)
        {
            var command = new Command();

            try
            {
                command = JsonConvert.DeserializeObject<Command>(input);
                return command;
            }
            catch(Exception e)
            {
                System.Console.WriteLine("An Error Occurred while parsing string. Details: "+e);
            }

            return command;
        }
    }
}
