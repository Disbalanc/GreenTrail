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
    
    public partial class Norm
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Norm()
        {
            this.Contemplation = new HashSet<Contemplation>();
        }
    
        public long id_norm { get; set; }
        public Nullable<long> id_type { get; set; }
        public string name { get; set; }
        public string norma { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contemplation> Contemplation { get; set; }
    }
}