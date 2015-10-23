using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Nest;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace ESSearchBoxSample.Models
{
    // You need to set IdProperty if you are using an id without name "id"
    [ElasticType(IdProperty = "DocumentId")]
    public class DocumentModel
    {
        //DocumentId will be used as id, no need to index it as a property
        [JsonIgnore]
        [Key]
        public int DocumentId { get; set; }

        [Required(ErrorMessage = "A Document Name is required")]
        [StringLength(maximumLength:255,MinimumLength = 1)]
        public string Name { get; set; }

        [Required(ErrorMessage = "An Document Text is required")]
        [StringLength(maximumLength: 255, MinimumLength = 1)]
        public string Text { get; set; }
    }
}