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
    
    public partial class Sample
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sample()
        {
            this.Contemplation = new HashSet<Contemplation>();
        }
    
        public long id_sample { get; set; }
        public Nullable<long> id_region { get; set; }
        public Nullable<long> id_user { get; set; }
        public string articul { get; set; }
        public Nullable<long> id_type { get; set; }
        public Nullable<System.DateTime> date_sample { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contemplation> Contemplation { get; set; }
        public virtual Region Region { get; set; }
        public virtual Type Type { get; set; }
        public virtual Users Users { get; set; }
    }
}
