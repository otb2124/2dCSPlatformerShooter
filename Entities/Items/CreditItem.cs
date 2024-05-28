using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Entities.items
{

    public class CreditItem : Item
    {
       
        public CreditItem() 
        { 
            this.value = 1;
            this.isStackable = true;
            this.amount = 1;
            this.Name = "Credit";
        }
    }
}
