using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Data.Interfaces
{
    public interface IEntity<out T>
    {
        T Id { get; }
    }
}
