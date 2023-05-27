using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace getQuote.Models
{
    public enum DocumentTypes
    {
        [Display(Name = "CPF")]
        CPF,

        [Display(Name = "RG")]
        RG
    }
}

