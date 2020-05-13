using LemonExam.Model;
using LemonExam.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LemonExam.Services
{
    public interface IDefaultServices
    {
        AccessObjectViewModel GetToken();
        int GetLineNumberERROR(Exception ex);
    }
 }
