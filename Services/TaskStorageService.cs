using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using TaskManagerDashboard_WPF.Model;

namespace TaskManagerDashboard_WPF.Services
{
    public class TaskStorageService
    {
        private const string FileName = @"C:\Users\muttenag\Documents\Visual Studio 18\tasks.json";

        public async Task SaveAsync(IEnumerable<TaskItem> tasks)
        {
            if (!File.Exists(FileName))
                File.Create(FileName).Dispose();
            var json = JsonSerializer.Serialize(tasks);
            await File.WriteAllTextAsync(FileName, json);
        }
    }
}