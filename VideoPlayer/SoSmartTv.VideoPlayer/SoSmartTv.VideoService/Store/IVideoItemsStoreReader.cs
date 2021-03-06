using System;
using System.Collections.Generic;
using SoSmartTv.VideoService.Dto;

namespace SoSmartTv.VideoService.Store
{
	public interface IVideoItemsStoreReader
	{
		IObservable<VideoItem> GetVideoItem(string title);

		IObservable<IList<VideoItem>> GetVideoItems(IEnumerable<string> titles);

		IObservable<VideoDetailsItem> GetVideoDetailsItem(int id);
	}
}