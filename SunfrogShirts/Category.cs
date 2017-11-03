using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunfrogShirts
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Category()
        {
            
        }

        public List<Category> getListCategory()
        {
            List<Category> ls = new List<Category>();
            ls.Add(new Category { Id = 8, Name = "Guys Tee" });
            ls.Add(new Category { Id = 34, Name = "Ladies Tee" });
            ls.Add(new Category { Id = 35, Name = "Youth Tee" });
            ls.Add(new Category { Id = 19, Name = "Hoodie" });
            ls.Add(new Category { Id = 27, Name = "Sweat Shirt" });
            ls.Add(new Category { Id = 50, Name = "Guys V-Neck" });
            ls.Add(new Category { Id = 116, Name = "Ladies V-Neck" });
            ls.Add(new Category { Id = 118, Name = "Unisex Tank Top" });
            ls.Add(new Category { Id = 119, Name = "Unisex Long Sleeve" });
            ls.Add(new Category { Id = 120, Name = "Leggings" });
            ls.Add(new Category { Id = 128, Name = "Coffee Mug (colored)" });
            ls.Add(new Category { Id = 129, Name = "Coffee Mug (white)" });
            ls.Add(new Category { Id = 145, Name = "Coffee Mug (color change)" });
            ls.Add(new Category { Id = 137, Name = "Posters 16x24" });
            ls.Add(new Category { Id = 138, Name = "Posters 24x16" });
            ls.Add(new Category { Id = 139, Name = "Posters 11x17" });
            ls.Add(new Category { Id = 140, Name = "Posters 17x11" });
            ls.Add(new Category { Id = 143, Name = "Canvas 16x20" });
            ls.Add(new Category { Id = 147, Name = "Hat" });
            ls.Add(new Category { Id = 148, Name = "Trucker Cap" });

            return ls;
        }
    }
}
