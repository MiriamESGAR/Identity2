using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FabricaElToro.Infrastructure
{
    public class FabricaElToroContext : DbContext 
    {
        public FabricaElToroContext(DbContextOptions<FabricaElToroContext> options)
            :base(options)
        {

        }
    }
}
