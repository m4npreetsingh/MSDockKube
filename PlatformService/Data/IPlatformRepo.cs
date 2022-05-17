﻿using PlatformService.Models;

namespace PlatformService.Data
{
    public interface IPlatformRepo
    {
        bool SaveChanges();
        IEnumerable<Platform> GetAll();
        Platform GetPlatformById(int id);
        void CreatePlatForm(Platform platform);
    }
}
