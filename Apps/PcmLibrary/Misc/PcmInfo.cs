﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcmHacking
{
    public enum PcmType
    {
        Undefined = 0, // required for failed osid test on binary file
        P01_P59,
        P04_Intel,
        P04_AMD,
        P10,
        P12,
        LB7,
        LLY,
        BLACKBOX
    }

    /// <summary>
    /// This combines various metadata about whatever PCM we've connected to.
    /// </summary>
    /// <remarks>
    /// This was ported from the LS1 flashing utility originally posted at pcmhacking.net.
    /// </remarks>
    public class PcmInfo
    {
        /// <summary>
        /// Operating system ID.
        /// </summary>
        public uint OSID { get; private set; }

        /// <summary>
        /// Descriptive text.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Indicates whether this PCM is supported by the app.
        /// </summary>
        public bool IsSupported { get; private set; }

        /// <summary>
        /// Indicates how to validate files before writing.
        /// </summary>
        public PcmType ValidationMethod { get; private set; }

        /// <summary>
        /// What type of hardware it is
        /// </summary>
        public PcmType HardwareType { get; private set; }

        /// <summary>
        /// Name of the kernel file to use.
        /// </summary>
        public string KernelFileName { get; private set; }

        /// <summary>
        /// Base address to begin writing the kernel to.
        /// </summary>
        public int KernelBaseAddress { get; private set; }

        /// <summary>
        /// Base address to begin reading or writing the ROM contents.
        /// </summary>
        public int ImageBaseAddress { get; private set; }

        /// <summary>
        /// Size of the ROM.
        /// </summary>
        public int ImageSize { get; private set; }

        /// <summary>
        /// Size of the ROM.
        /// </summary>
        public int RAMSize { get; private set; }

        /// <summary>
        /// Which key algorithm to use to unlock the PCM.
        /// </summary>
        public int KeyAlgorithm { get; private set; }

        /// <summary>
        /// Does PCM's kernel support flash segment checksum?
        /// </summary>
        public bool ChecksumSupport { get; private set; }

        /// <summary>
        /// Does PCM's kernel support flash chip identification?
        /// </summary>
        public bool FlashIDSupport { get; private set; }

        /// <summary>
        /// Does PCM's kernel support version number identification?
        /// </summary>
        public bool KernelVersionSupport { get; private set; }

        /// <summary>
        /// PCM kernel max block size.
        /// </summary>
        public int KernelMaxBlockSize { get; private set; }

        /// <summary>
        /// Populate this object based on the given OSID.
        /// </summary>
        public PcmInfo(uint osid)
        {
            this.OSID = osid;

            // These defaults work for P01 and P59 hardware.
            // Differences are overwriten for other hardware and kernels.
            this.KernelFileName = "Kernel-P01.bin";
            this.KernelBaseAddress = 0xFF8000;
            this.RAMSize = 0x4DFF;
            this.ValidationMethod = PcmType.P01_P59;
            this.HardwareType = PcmType.P01_P59;
            this.ChecksumSupport = true;
            this.FlashIDSupport = true;
            this.KernelVersionSupport = true;
            this.KernelMaxBlockSize = 4096;

            // This will be overwritten for known-to-be-unsupported operating systems.
            this.IsSupported = true;

            switch (osid)
            {
                // LB7 Duramax EFI Live COS
                case 01337601:
                case 01710001:
                case 01887301:
                case 02444101:
                case 02600601:
                case 02685301:
                case 03904401:
                case 01337605:
                case 01710005:
                case 01887305:
                case 02444105:
                case 02600605:
                case 02685305:
                case 03904405:
                    this.KeyAlgorithm = 2;
                    this.Description = "LB7 EFILive COS";
                    this.ImageBaseAddress = 0x0;
                    this.ImageSize = 512 * 1024;
                    this.HardwareType = PcmType.LB7;
                    break;

                // LB7 Duramax service no 9388505
                case 15063376:
                case 15188873:
                case 15097100:
                    this.KeyAlgorithm = 2;
                    this.Description = "LB7 9388505";
                    this.ImageBaseAddress = 0x0;
                    this.ImageSize = 512 * 1024;
                    this.HardwareType = PcmType.LB7;
                    break;

                // LB7 Duramax service no 12210729
                case 15094441:
                case 15085499:
                case 15166853:
                case 15186006:
                case 15189044:
                    this.KeyAlgorithm = 2;
                    this.Description = "LB7 12210729";
                    this.ImageBaseAddress = 0x0;
                    this.ImageSize = 512 * 1024;
                    this.HardwareType = PcmType.LB7;
                    break;

                // LLY Duramax service no 12244189 - 1mbyte?
                case 15141668:
                case 15193885:
                case 15228758:
                case 15231599:
                case 15231600:
                case 15879103:
                case 15087230:
                    this.IsSupported = false;
                    this.KeyAlgorithm = 2;
                    this.Description = "LLY 12244189";
                    this.ImageBaseAddress = 0x0;
                    this.ImageSize = 1024 * 1024;
                    this.HardwareType = PcmType.LLY;
                    break;

                // LL7 Duramax EFI Live Cos
                case 04166801:
                case 04166805:
                case 05160001:
                case 05160005:
                case 05388501:
                case 05388505:
                case 05875801:
                case 05875805:
                    this.IsSupported = false;
                    this.KeyAlgorithm = 2;
                    this.Description = "LLY EFILive COS";
                    this.ImageBaseAddress = 0x0;
                    this.ImageSize = 1024 * 1024;
                    this.HardwareType = PcmType.LLY;
                    break;

                // VCM Suite COS
                case 1251001:
                    this.KeyAlgorithm = 3;
                    this.Description = "VCM Suite 2 Bar";
                    this.ImageBaseAddress = 0x0;
                    this.ImageSize = 512 * 1024;
                    break;

                case 1261001:
                    this.KeyAlgorithm = 4;
                    this.Description = "VCM Suite 3 Bar";
                    this.ImageBaseAddress = 0x0;
                    this.ImageSize = 512 * 1024;
                    break;

                case 1271001:
                    this.KeyAlgorithm = 5;
                    this.Description = "VCM Suite Mafless";
                    this.ImageBaseAddress = 0x0;
                    this.ImageSize = 512 * 1024;
                    break;

                case 1281001:
                    this.KeyAlgorithm = 6;
                    this.Description = "VCM Suite MAF RTT";
                    this.ImageBaseAddress = 0x0;
                    this.ImageSize = 512 * 1024;
                    break;

                case 1271002:
                    this.KeyAlgorithm = 7;
                    this.Description = "VCM Suite Mafless";
                    this.ImageBaseAddress = 0x0;
                    this.ImageSize = 512 * 1024;
                    break;

                case 1251002:
                    this.KeyAlgorithm = 8;
                    this.Description = "VCM Suite 2 Bar";
                    this.ImageBaseAddress = 0x0;
                    this.ImageSize = 512 * 1024;
                    break;

                case 1261002:
                    this.KeyAlgorithm = 9;
                    this.Description = "VCM Suite MAF RTT";
                    this.ImageBaseAddress = 0x0;
                    this.ImageSize = 512 * 1024;
                    break;

                case 1281002:
                    this.KeyAlgorithm = 10;
                    this.Description = "VCM Suite 3 Bar";
                    this.ImageBaseAddress = 0x0;
                    this.ImageSize = 512 * 1024;
                    break;
                case 1271003:
                    this.KeyAlgorithm = 11;
                    this.Description = "VCM Suite Mafless";
                    this.ImageBaseAddress = 0x0;
                    this.ImageSize = 512 * 1024;
                    break;

                case 1251003:
                    this.KeyAlgorithm = 12;
                    this.Description = "VCM Suite 2 Bar";
                    this.ImageBaseAddress = 0x0;
                    this.ImageSize = 512 * 1024;
                    break;

                case 1261003:
                    this.KeyAlgorithm = 13;
                    this.Description = "VCM Suite 3 Bar";
                    this.ImageBaseAddress = 0x0;
                    this.ImageSize = 512 * 1024;
                    break;

                case 1281003:
                    this.KeyAlgorithm = 14;
                    this.Description = "VCM Suite MAF RTT";
                    this.ImageBaseAddress = 0x0;
                    this.ImageSize = 512 * 1024;
                    break;

                //------- HPT COS -----------
                case 1250013:
                case 1250018:
                case 1251005:
                case 1251006:
                case 1251008:
                case 1251010:
                case 1251011:
                case 1251012:
                case 1251014:
                case 1251016:
                case 1251017:
                case 1260006:
                case 1260011:
                case 1261005:
                case 1261008:
                case 1261014:
                case 1261016:
                case 1270013:
                case 1270017:
                case 1271005:
                case 1271006:
                case 1271008:
                case 1271010:
                case 1271011:
                case 1271012:
                case 1271014:
                case 1271016:
                case 1271018:
                case 1281005:
                case 1281006:
                case 1281008:
                case 1281010:
                case 1281011:
                case 1281012:
                case 1281014:
                case 1281016:
                case 1281918:
                    this.Description = "Unknown VCM Suite COS";
                    this.ImageBaseAddress = 0x0;
                    this.ImageSize = 512 * 1024;
                    break;

                //-------------------------

                // 9354896
                case 9360360:
                case 9360361:
                case 9361140:
                case 9363996:
                case 9365637:
                case 9373372:
                case 9379910:
                case 9381344:
                case 12205612:
                case 12584929:
                case 12593359:
                case 12597506:
                case 16253027:
                    this.KeyAlgorithm = 40;
                    this.Description = "9354896";
                    this.ImageBaseAddress = 0x0;
                    this.ImageSize = 512 * 1024;
                    break;

                // 12200411
                case 12202088: // main 2000/2001 OS
                case 12206871:
                case 12208322:
                case 12209203:
                case 12212156:
                case 12216125:
                case 12221588:
                case 12225074: // main 2003/2004 OS
                case 12593358:
                    this.KeyAlgorithm = 40;
                    this.Description = "12200411";
                    this.ImageBaseAddress = 0x0;
                    this.ImageSize = 512 * 1024;
                    break;

                case 01250001:
                case 01290001:
                case 02020002:
                case 02040001:
                case 03150002:
                case 04072901:
                case 04073101:
                case 04110003:
                case 05120003:
                case 01250002:
                case 01290002:
                case 02020003:
                case 02040002:
                case 03150003:
                case 04072902:
                case 04073102:
                case 04140001:
                case 01250003:
                case 01290003:
                case 02020005:
                case 02040003:
                case 03170001:
                case 04072903:
                case 04073103:
                case 04140002:
                case 01270001:
                case 01290005:
                case 02030001:
                case 03110001:
                case 03190001:
                case 04073001:
                case 04080001:
                case 04140003:
                case 01270002:
                case 02010001:
                case 02030002:
                case 03130001:
                case 03190002:
                case 04073002:
                case 04110001:
                case 05120001:
                case 01270003:
                case 02020001:
                case 02030003:
                case 03150001:
                case 03190003:
                case 04073003:
                case 04110002:
                case 05120002:
                    this.KeyAlgorithm = 40;
                    string type = osid.ToString();
                    switch (Convert.ToInt32(type, 10))
                    {
                        case 1:
                            this.Description = "EFI Live COS1";
                            break;

                        case 2:
                            this.Description = "EFI Live COS2";
                            break;

                        case 3:
                            this.Description = "EFI Live COS3";
                            break;

                        case 5:
                            this.Description = "EFI Live COS5";
                            break;

                        default:
                            this.Description = "EFI Live COS";
                            break;
                    }
                    this.ImageBaseAddress = 0x0;
                    this.ImageSize = 512 * 1024;
                    break;

                // 1Mb pcms
                // GMC Sierra service number 12589463 
                case 12591725:
                case 12592618:
                case 12593555:
                case 12606961:
                case 12612115:
                    this.KeyAlgorithm = 40;
                    this.Description = "12589463";
                    this.ImageSize = 1024 * 1024;
                    break;

                //case 1250052:
                //case 1250058:
                //case 4073103:
                case 12564440:
                case 12585950:
                case 12588804:
                case 12592425:
                case 12592433: //Aussie
                case 12606960:
                case 12612114:
                    this.KeyAlgorithm = 40;
                    this.Description = "12586242";
                    this.ImageSize = 1024 * 1024;
                    break;

                // usa 12586243
                case 12587603:
                case 12587604:
                case 76030003:
                case 76030004:
                case 76030005:
                case 76030006:
                case 76030007:
                case 76030008:
                case 76030009:
                    this.KeyAlgorithm = 40;
                    this.Description = "12586243";
                    this.ImageSize = 1024 * 1024;
                    break;

                // not sure 12582605
                case 12578128:
                case 12579405:
                case 12580055:
                case 12593058:
                    this.KeyAlgorithm = 40;
                    this.Description = "12582605";
                    this.ImageSize = 1024 * 1024;
                    break;

                // not sure 12582811
                case 12587811:
                case 12605114:
                case 12606807:
                case 12608669:
                case 12613245:
                case 12613246:
                case 12613247:
                case 12619623:
                    this.KeyAlgorithm = 40;
                    this.Description = "12582811";
                    this.ImageSize = 1024 * 1024;
                    break;

                // not sure 12602802
                case 12597120:
                case 12613248:
                case 12619624:
                    this.KeyAlgorithm = 40;
                    this.Description = "12602802";
                    this.ImageSize = 1024 * 1024;
                    break;

                case 9355699:
                case 9365095:
                case 16263425: // 9366810 'black box'
                    this.IsSupported = false;
                    this.KeyAlgorithm = 15;
                    this.Description = "'Black Box' 9366810";
                    this.ImageBaseAddress = 0x0;
                    this.ImageSize = 512 * 1024;
                    this.HardwareType = PcmType.BLACKBOX;
                    break;

                // P04_Intel
                case 12202127:
                case 12207868:
                case 12214378:
                case 12580147:
                case 16243026:
                    this.KeyAlgorithm = 14;
                    this.Description = "1998-2005 V6";
                    this.ImageBaseAddress = 0x0;
                    this.ImageSize = 512 * 1024;
                    this.IsSupported = true;
                    this.KernelBaseAddress = 0xFF9090;
                    this.ValidationMethod = PcmType.P04_Intel;
                    this.HardwareType = PcmType.P04_Intel;
                    this.KernelFileName = "Kernel-P04-Intel.bin";
                    this.ChecksumSupport = false;
                    this.KernelVersionSupport = false;
                    this.FlashIDSupport = false;
                    this.KernelMaxBlockSize = 2048;
                    break;

                // P04_AMD
                case 12583373:
                case 12594316:
                case 12594535:
                    this.KeyAlgorithm = 14;
                    this.Description = "1998-2005 V6";
                    this.ImageBaseAddress = 0x0;
                    this.ImageSize = 512 * 1024;
                    this.IsSupported = true;
                    this.KernelBaseAddress = 0xFF9090;
                    this.ValidationMethod = PcmType.P04_AMD;
                    this.HardwareType = PcmType.P04_AMD;
                    this.KernelFileName = "Kernel-P04-AMD.bin";
                    this.ChecksumSupport = false;
                    this.KernelVersionSupport = false;
                    this.FlashIDSupport = false;
                    this.KernelMaxBlockSize = 512;
                    break;

                // P10
                case 12213305:
                case 12571911:
                case 12575262:
                case 12577956:
                case 12579238:
                case 12579357:
                case 12584138:
                case 12584594:
                case 12587430:
                case 12587608:
                case 12588012:
                case 12589825:
                case 12590965:
                case 12595726:
                case 12597031:
                case 12623317:
                    this.KernelFileName = "Kernel-P10.bin";
                    this.KernelBaseAddress = 0xFFB800; // Or FFA000? https://pcmhacking.net/forums/viewtopic.php?f=42&t=7742&start=450#p115622
                    this.RAMSize = 0x2800; // or 4000?
                    this.IsSupported = true;
                    this.KeyAlgorithm = 66;
                    this.Description = "P10";
                    this.ImageBaseAddress = 0x0;
                    this.ImageSize = 512 * 1024;
                    this.ValidationMethod = PcmType.P10;
                    this.HardwareType = PcmType.P10;
                    break;

                // P12 1m
                case 12587007:
                case 12588651:
                case 12589166:
                case 12589312:
                case 12589586:
                case 12593533:
                case 12597778:
                case 12597978:
                case 12601774:
                case 12601904: // Saturn I4
                case 12605256:
                case 12605261:
                case 12610624:
                case 12623279:
                case 12627882:
                case 12627884:
                case 12631085:
                // LL8 - Atlas I6 (4200) P12
                case 12604440:
                case 12606400:
                // L52 - Atlas I5 (3500) P12
                case 12606374:
                case 12606375:
                // LK5 - Atlas I4 (2800) P12
                case 12627883:
                // Saturn 1mb P12
                    this.KernelFileName = "Kernel-P12.bin";
                    this.KernelBaseAddress = 0xFF2000; // or FF0000? https://pcmhacking.net/forums/viewtopic.php?f=42&t=7742&start=450#p115622
                    this.RAMSize= 0x6000;
                    this.IsSupported = true;
                    this.KeyAlgorithm = 91;
                    this.Description = "P12 1m (Atlas I4/I5/I6)";
                    this.ImageBaseAddress = 0x0;
                    this.ImageSize = 1024 * 1024;
                    this.ValidationMethod = PcmType.P12;
                    this.HardwareType = PcmType.P12;
                    break;

                // P12 2m - See: https://pcmhacking.net/forums/viewtopic.php?f=42&t=7742&start=470#p115747
                case 12609805:
                case 12611642:
                case 12613422:
                case 12618164:
                case 12627885:
                    this.KernelFileName = "Kernel-P12.bin";
                    this.KernelBaseAddress = 0xFF2000; // or FF0000? https://pcmhacking.net/forums/viewtopic.php?f=42&t=7742&start=450#p115622
                    this.RAMSize = 0x6000;
                    this.IsSupported = true;
                    this.KeyAlgorithm = 91;
                    this.Description = "P12 2m (Atlas I4/I5/I6)";
                    this.ImageBaseAddress = 0x0;
                    this.ImageSize = 2048 * 1024;
                    this.ValidationMethod = PcmType.P12;
                    this.HardwareType = PcmType.P12;
                    break;

                default:
                    this.IsSupported = false;
                    this.KeyAlgorithm = 40;
                    this.Description = "Unknown";
                    this.ImageBaseAddress = 0x0;
                    this.ImageSize = 0;
                    break;
            }
        }
    }
}
