using System.IO;
using System.Runtime.InteropServices;

namespace Lain.Core
{
	/// <summary>
	///     Класс, расширяющий BinaryReader и BinaryWriter для работы с структурами
	/// </summary>
	public static class BinaryOperations
	{
		/// <summary>
		///     Gets the array.
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		/// <param name="obj">The object.</param>
		/// <param name="deleteOld">if set to <c>true</c> [delete old].</param>
		/// <returns></returns>
		public static byte[] GetArray<T>(T obj, bool deleteOld = true)
		{
			var len = Marshal.SizeOf<T>();
			var arr = new byte[len];
			var ptr = Marshal.AllocHGlobal(len);
			Marshal.StructureToPtr(obj, ptr, deleteOld);
			Marshal.Copy(ptr, arr, 0, len);
			Marshal.FreeHGlobal(ptr);
			return arr;
		}

		/// <summary>
		///     Чтение структуры из потока
		/// </summary>
		/// <typeparam name="T">Тип структуры</typeparam>
		/// <param name="reader">BinaryReader</param>
		/// <returns>
		///     Обьект, который читается
		/// </returns>
		public static T ReadStruct<T>(this BinaryReader reader) where T : struct
		{
			var rawData = reader.ReadBytes(Marshal.SizeOf<T>());
			var handle = GCHandle.Alloc(rawData, GCHandleType.Pinned);
			var returnObject = Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());
			handle.Free();
			return returnObject;
		}

		/// <summary>
		///     Запись структуры в поток
		/// </summary>
		/// <typeparam name="T">Тип структуры</typeparam>
		/// <param name="writer">BinaryWriter</param>
		/// <param name="obj">Обьект, который записываем</param>
		public static void WriteStruct<T>(this BinaryWriter writer, T obj) where T : struct
		{
			writer.Write(GetArray(obj, false));
		}
	}
}