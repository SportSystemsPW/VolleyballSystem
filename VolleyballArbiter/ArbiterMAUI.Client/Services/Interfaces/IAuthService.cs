﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbiterMAUI.Client.Services.Interfaces
{
    public interface IAuthService
    {
        Task<HttpResponseMessage> CheckRefereeKey();
    }
}
