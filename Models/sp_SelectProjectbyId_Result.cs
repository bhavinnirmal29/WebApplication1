//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;

    public class EmployeeModel
    {
        public List<sp_SelectProjectbyId_Result> emps { get; set; }
    }
    public partial class sp_SelectProjectbyId_Result
    {
        public int projid { get; set; }
        public string projname { get; set; }
        public string domain { get; set; }
    }
}
