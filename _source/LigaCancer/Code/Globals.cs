using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Code
{
    public class Globals
    {
       
        public enum Sex
        {
            Male = 0,
            Female = 1,
            NotSpecified = 2,

        }

        public enum CivilState
        {
            Single,
            Married,
            Widowed,
            Divorced,
            Engaged,
            Relationship,
            StableUnion,
            Cohabitating
        }

        public enum PhoneType
        {
            landline,
            cellphone
        }
    }
}
