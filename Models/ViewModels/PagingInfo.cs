﻿namespace ESHOP.Models.ViewModels
{
    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrenPage { get; set; }
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / CurrenPage);
    }
}
