using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SampleTest.Net {
	public static class IPAddressExtensions {
		/// <summary>
		/// 論理AND演算
		/// </summary>
		/// <param name="address"></param>
		/// <param name="mask"></param>
		/// <returns></returns>
		public static IPAddress IPv4LogicalAnd(this IPAddress address, IPAddress mask) {
			// IPv4のみ
			if (address.AddressFamily != AddressFamily.InterNetwork) {
				throw new ArgumentException(nameof(address));
			}
			if (mask.AddressFamily != AddressFamily.InterNetwork) {
				throw new ArgumentException(nameof(mask));
			}

			var addressBytes = address.GetAddressBytes();
			var maskBytes = mask.GetAddressBytes();
			var resultBytes = addressBytes.Zip(maskBytes, (addressByte, maskByte) => (byte)(addressByte & maskByte)).ToArray();
			return new IPAddress(resultBytes);
		}

		/*
		// todo:
		public static IPAddress GetIPv4NetworkAddress(this IPAddress address, int prefix) {
			return IPAddress.None;
		}

		public static IPAddress GetIPv4NetworkAddress(this IPAddress address, IPAddress mask) {
			return IPAddress.None;
		}
		*/

		/*
		public static bool IsIPv4InSubnet(this IPAddress address, IPAddress subnet, int prefix) {
			return false;
		}

		public static bool IsIPv4InSubnet(this IPAddress address, IPAddress subnet, IPAddress mask) {
			return false;
		}
		*/

		private static readonly (byte[] start, byte[] end)[] _ipv4PrivateByteRanges
			= new[] {
				// 参考
				// https://www.nic.ad.jp/ja/translation/rfc/1918.html
				// 10.0.0.0 - 10.255.255.255 (10/8 prefix)
				// 172.16.0.0 - 172.31.255.255 (172.16/12 prefix)
				// 192.168.0.0 - 192.168.255.255 (192.168/16 prefix)
				(new byte [] { 10, 0, 0, 0 }, new byte [] { 10, 255, 255, 255 }),
				(new byte [] { 172, 16, 0, 0 }, new byte [] { 172, 31, 255, 255 }),
				(new byte [] { 192, 168, 0, 0 }, new byte [] { 192, 168, 255, 255 }),
			};

		/// <summary>
		/// IPv4のプライベートIPアドレスか
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public static bool IsIPv4Private(this IPAddress address) {
			// IPv4のみ
			if (address.AddressFamily != AddressFamily.InterNetwork) {
				throw new ArgumentException(nameof(address));
			}

			var target = address.GetAddressBytes();
			foreach (var (start, end) in _ipv4PrivateByteRanges) {
				if (Enumerable.Range(0, 4).All(index => target[index] >= start[index] && target[index] <= end[index])) {
					return true;
				}
			}

			return false;
		}
	}
}
