﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orleans.Grains
{
    /// <summary>
    /// Orleans grain communication interface IHello
    /// </summary>
    public interface IHelloA : Orleans.IGrainWithIntegerKey
    {
        Task<string> SayHello(string greeting);
        Task<string> TalkToMyself(string greeting);
    }
}
