using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MissCore.Abstractions;

namespace MissBot.Interfaces
{
    public interface IContext<in TScope>
    {
        IContext<TScope> With<T>(T value, string name = null) where T : class;
        IContext<T> GetContextOf<T>(string name = null);
        public T GetContext<T>(bool createIfNotExists = true, string name = null) where T : class, IHandleContext;
    }
}
