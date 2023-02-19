/* Unmerged change from project 'MissBot.Abstractions (netstandart2.0)'
Before:
namespace MissCore.Abstractions
After:
using MissBot.Abstractions;
using MissCore;
using MissCore.Abstractions;

namespace MissCore.Abstractions
*/

namespace MissBot.Abstractions
{

    public interface IContext
    {
        T Get<T>(Predicate<string> filter = null);
        T Get<T>();
        T Set<T>(T value, string name = null);
        T Get<T>(string name);
    }
}
