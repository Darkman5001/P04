﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PcmHacking
{
    /// <summary>
    /// This class is responsible for generating and parsing the messages that the app exchanges with the PCM.
    /// </summary>
    /// <remarks>
    /// The messages generated by this class are byte-for-byte exactly what the PCM 
    /// receives, with the exception of the CRC byte at the end. CRC bytes must be 
    /// added by the currently-selected Device class if the actual device doesn't add 
    /// the CRC byte automatically.
    ///
    /// Some devices will require these messages to be translated according to the specific
    /// device's protocol - that too is the job of the currently-selected Device class.
    /// </remarks>
    public partial class Protocol
    {
        /// <summary>
        /// Initialize a new instance of the Protocol class.
        /// </summary>
        public Protocol()
        {
            // It might be tempting to make PcmInfo a parameter of the
            // constructor, so that it can be stored in a member variable
            // and referenced when creating messages that are different for
            // different PCMs. 
            //
            // But that creates a chicken-and-egg problem. You need the OSID
            // in order to initialize the PcmInfo, and you need the protocol
            // to query the OSID, so the Protocol needs to be created before
            // the PcmInfo is created.
        }
    }
}
