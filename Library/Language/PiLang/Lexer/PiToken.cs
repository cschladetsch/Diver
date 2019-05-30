﻿using Pyro.Language.Impl;

namespace Pyro.Language.Lexer
{
    /// <summary>
    /// A Pi-lang token.
    /// </summary>
    public class PiToken
        : TokenBase<EPiToken>
        , ITokenNode<EPiToken>
    {
        public PiToken()
        {
            _type = EPiToken.None;
        }

        public PiToken(EPiToken type, Slice slice)
            : base(type, slice)
        {
        }
    }
}
