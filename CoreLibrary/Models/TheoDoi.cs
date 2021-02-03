﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CoreLibrary.Models
{
    [Table("theo_doi")]
    public class TheoDoi
    {
        [Key]
        public Guid TheoDoiID { get; set; }

        //Tập khóa ngoại
        public Guid TruyenID { get; set; }
        public Truyen Truyen { get; set; }

        //Tập khóa ngoại
        public Guid UserID { get; set; }
        public User User { get; set; }
    }
}
