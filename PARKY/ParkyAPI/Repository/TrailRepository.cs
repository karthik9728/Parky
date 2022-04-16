using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace ParkyAPI.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDbContext _db;

        public TrailRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool CreateTrail(Trail trail)
        {
            _db.Trail.Add(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _db.Remove(trail);
            return Save();
        }

        public Trail GetTrail(int trailId)
        {
            return _db.Trail.Include(x => x.NationalPark).FirstOrDefault(x => x.Id == trailId);
        }

        public ICollection<Trail> GetTrails()
        {
            return _db.Trail.Include(x => x.NationalPark).OrderBy(x => x.Name).ToList();
        }

        public ICollection<Trail> GetTrailsInNationalPark(int npId)
        {
            return _db.Trail.Include(x => x.NationalPark).Where(x => x.NationalParkId == npId).ToList();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool TrailExists(string name)
        {
            return _db.Trail.Any(x => x.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool TrailExists(int trailId)
        {
            return _db.Trail.Any(x => x.Id == trailId);
        }

        public bool UpdateTrail(Trail trail)
        {
            _db.Trail.Update(trail);
            return Save();
        }
    }
}
