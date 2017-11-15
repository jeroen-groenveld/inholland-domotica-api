﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Web_API.Models
{
    public class Blog
    {
        //Primary Key
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BlogId { get; set; }

        [MaxLength(250)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Url { get; set; }

        //The Identity option specifies that the value will only be generated by the database when a value is first added to the database.
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime created_at { get; set; }

        // The Computed option specifies that the property's value will be generated by the database when the value is first saved, and subsequently regenerated every time the value is updated.
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime udpated_at { get; set; }


        public List<Post> Posts { get; set; } = new List<Post>();
    }
}