using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Model.Models.Request;

public class NoteRequest
{
    [Required(ErrorMessage = "Note is required")]
    public string Note { get; set; }
}
