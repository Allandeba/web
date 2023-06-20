using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace getQuote.Models
{
    public enum DocumentTypes : int
    {
        CPF = 0,
        RG = 1
    }

    public enum SelectDefault
    {
        None
    }
}
