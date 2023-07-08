using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace getQuote.Models
{
    public enum DocumentTypes : int
    {
        CPF,
        RG
    }

    public enum SelectDefault
    {
        None
    }
}
