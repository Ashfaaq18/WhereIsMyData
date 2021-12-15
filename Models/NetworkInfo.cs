﻿
using System;
using System.Diagnostics;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Microsoft.Diagnostics.Tracing.Session;
using WhereIsMyData.ViewModels;

namespace WhereIsMyData.Models
{
    public class NetworkInfo
    {
        private static DataUsageSummaryVM dusvm;
        private static DataUsageDetailedVM dudvm;
        public NetworkInfo(ref DataUsageSummaryVM dusvm_ref, ref DataUsageDetailedVM dudvm_ref)
        {
            dusvm = dusvm_ref;
            dudvm = dudvm_ref;
            TotalBytesRecv = 0;

            using (TraceEventSession kernelSession = new TraceEventSession(KernelTraceEventParser.KernelSessionName))
            {
                Console.CancelKeyPress += new ConsoleCancelEventHandler((object sender, ConsoleCancelEventArgs cancelArgs) =>
                {
                    Console.WriteLine("Control C pressed");     // Note that if you hit Ctrl-C twice rapidly you may be called concurrently.  
                    kernelSession.Dispose();                          // Note that this causes Process() to return.  
                    cancelArgs.Cancel = true;                   // This says don't abort, since Process() will return we can terminate nicely.   
                });

                kernelSession.EnableKernelProvider(KernelTraceEventParser.Keywords.NetworkTCPIP);

                kernelSession.Source.Kernel.TcpIpRecv += Kernel_TcpIpRecv;
                kernelSession.Source.Kernel.TcpIpRecvIPV6 += Kernel_TcpIpRecvIPV6;
                kernelSession.Source.Kernel.UdpIpRecv += Kernel_UdpIpRecv;
                kernelSession.Source.Kernel.UdpIpRecvIPV6 += Kernel_UdpIpRecvIPV6;

                kernelSession.Source.Process();
            }
        }


        public static ulong TotalBytesRecv { get; set; }
        private static void Kernel_UdpIpRecv(UdpIpTraceData obj)
        {
            TotalBytesRecv += (ulong)obj.size;
            dusvm.CurrentSessionData = TotalBytesRecv / (1024) ;
            dudvm.EditProcessInfo(obj.ProcessName, (ulong)obj.size);
            //Debug.WriteLine("UDP pid {0}, {1}", obj.ProcessID, obj.size);
        }

        private void Kernel_UdpIpRecvIPV6(UpdIpV6TraceData obj)
        {
            TotalBytesRecv += (ulong)obj.size;
            dusvm.CurrentSessionData = TotalBytesRecv / (1024);
            dudvm.EditProcessInfo(obj.ProcessName, (ulong)obj.size);
        }

        private static void Kernel_TcpIpRecv(TcpIpTraceData obj)
        {
            TotalBytesRecv += (ulong)obj.size;

            dusvm.CurrentSessionData = TotalBytesRecv / (1024);

            dudvm.EditProcessInfo(obj.ProcessName, (ulong)obj.size);
            // Debug.WriteLine("TCP pid {0}, {1}", obj.ProcessID, obj.size);
        }

        private void Kernel_TcpIpRecvIPV6(TcpIpV6TraceData obj)
        {
            TotalBytesRecv += (ulong)obj.size;
            dusvm.CurrentSessionData = TotalBytesRecv / (1024);
            dudvm.EditProcessInfo(obj.ProcessName, (ulong)obj.size);
        }

        private static void processStarted(ProcessTraceData data)
        {
           // Debug.WriteLine("start");
        }
        private static void processStopped(ProcessTraceData data)
        {
           // Debug.WriteLine("stop");
        }

    }
}
