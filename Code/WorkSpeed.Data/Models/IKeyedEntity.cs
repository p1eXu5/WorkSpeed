using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Data.Models
{
    public interface IKeyedEntity<out T> : IEntity
    {
        T Id { get; }
    }
}
