using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CukCuk.Core.CustomAttrs
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MISADbColumnNotMatch : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class MISAColumnForImport : Attribute
    {

    }
}
