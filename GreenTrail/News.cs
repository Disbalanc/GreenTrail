//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GreenTrail
{
    using System;
    using System.Collections.Generic;
    
    public partial class News
    {
        public long id_news { get; set; }
        public Nullable<long> id_user { get; set; }
        public string heading { get; set; }
        public string text { get; set; }
        public Nullable<System.DateTime> data_time { get; set; }
        public string image { get; set; }
    
        public virtual Users Users { get; set; }
    }
}