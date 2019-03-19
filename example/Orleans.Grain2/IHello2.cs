﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orleans.Grains2
{
    /// <summary>
    /// Orleans grain communication interface IHello
    /// </summary>
    public interface IHello2 : Orleans.IGrainWithIntegerKey
    {
        Task<string> SayHello(string greeting);
    }
}
