# Complete YouTube Downloader 🎬

A powerful, feature-rich YouTube downloader with both GUI and CLI interfaces, smart file verification, and robust download resumption capabilities.

![YouTube Downloader](https://github.com/ibhoqba/YouTubeDownloader/releases/tag/YouTubeDownloader)
![Python](https://img.shields.io/badge/Python-3.8%2B-blue?style=for-the-badge&logo=python)

## ✨ Features

### 🎯 Core Functionality
- **Dual Interface**: Both GUI (Graphical User Interface) and CLI (Command Line Interface)
- **Smart Downloads**: Automatically skips already downloaded content using file size verification
- **Resume Support**: Continues interrupted downloads seamlessly
- **Batch Processing**: Download multiple videos/playlists from a text file

### 📹 Download Options
- **Multiple Resolutions**: Best, 1080p, 720p, 480p, 360p, or Audio-only
- **Playlist Support**: Download entire YouTube playlists with folder organization
- **Subtitles**: English closed captions and auto-generated subtitles
- **Format Selection**: MP4, MKV, WebM, and audio formats

### 🔍 Smart Verification
- **File Size Checking**: Compares downloaded file size with expected size
- **Fuzzy Matching**: Intelligent filename comparison to detect existing files
- **Configurable Tolerance**: Set completion threshold (80%, 90%, 95%, etc.)
- **Detailed Analysis**: Shows file completion percentage and size information

### 🛡️ Reliability Features
- **Network Resilience**: Automatic retry on network failures
- **Fragment Recovery**: Handles corrupted or missing video fragments
- **Progress Tracking**: Persistent download state across sessions
- **Error Handling**: Comprehensive error reporting and recovery

## 🚀 Installation

### Prerequisites
- Python 3.8 or higher
- pip (Python package manager)

### Step 1: Install Dependencies
```bash
pip install yt-dlp
```

### Step 2: Download the Application
Download `CompleteYouTubeDownloader.py` to your desired directory.

### Optional: FFmpeg (Recommended)
For best quality and format conversion, install FFmpeg:
- **Windows**: Download from [FFmpeg Official Site](https://ffmpeg.org/download.html)
- **Add to PATH** or place in the same directory as the script

## 📖 Usage

### GUI Mode (Recommended for Beginners)
```bash
python CompleteYouTubeDownloader.py --gui
```
_or simply:_
```bash
python CompleteYouTubeDownloader.py
```

### CLI Mode (For Advanced Users)
```bash
# Single video download
python CompleteYouTubeDownloader.py "https://youtube.com/watch?v=VIDEO_ID" -r 720p -s

# Playlist download
python CompleteYouTubeDownloader.py "https://youtube.com/playlist?list=PLAYLIST_ID" -r 1080p

# Batch download from file
python CompleteYouTubeDownloader.py -f urls.txt -o "G:\Videos" --size-tolerance 0.95

# Audio-only download
python CompleteYouTubeDownloader.py "URL" -r audio
```

## 🎮 GUI Interface Guide

### Main Window Components
1. **URL Input**: Paste YouTube video or playlist URL
2. **Output Directory**: Choose where to save downloads
3. **Resolution Selection**: Choose video quality
4. **Download Options**:
   - English Subtitles
   - Auto-generated Subtitles
   - Size Tolerance (completion threshold)
5. **Download Type**: Auto-detect, Single Video, or Playlist
6. **Action Buttons**:
   - 📊 Check File Status
   - ⬇️ Download
   - 📁 Batch Download
   - 🔄 Resume Failed
   - 🧹 Clear Log
7. **Download Log**: Real-time progress and status messages

### Using the GUI
1. **Single Download**:
   - Paste URL → Select options → Click "Download"
   
2. **Check File Status**:
   - Paste URL → Click "Check File Status" to see if file exists and completion status

3. **Batch Download**:
   - Create a text file with one URL per line
   - Click "Batch Download" → Select your file

4. **Resume Downloads**:
   - Use "Resume Failed" to continue interrupted downloads

## ⌨️ Command Line Options

| Option | Description | Default |
|--------|-------------|---------|
| `url` | YouTube video or playlist URL | - |
| `-f, --file` | Text file containing URLs (one per line) | - |
| `-o, --output` | Output directory for downloads | `downloads` |
| `-r, --resolution` | Video resolution: `best`, `1080p`, `720p`, `480p`, `360p`, `audio` | `best` |
| `-s, --subs` | Download English subtitles | `False` |
| `--size-tolerance` | Size tolerance ratio (0.8 = 80%) | `0.9` |
| `--gui` | Launch graphical interface | `False` |

## 📁 File Structure

```
downloads/
├── Single_Video_Title.mp4
├── Another_Video.mkv
└── Playlist_Name/
    ├── Video_1_In_Playlist.mp4
    ├── Video_2_In_Playlist.mp4
    └── ...
```

## 📝 URLs File Format

Create a text file with YouTube URLs (one per line):
```text
# My YouTube Downloads
https://www.youtube.com/watch?v=VIDEO_ID_1
https://www.youtube.com/watch?v=VIDEO_ID_2
https://www.youtube.com/playlist?list=PLAYLIST_ID_1
https://www.youtube.com/playlist?list=PLAYLIST_ID_2

# Comments start with hash
```

## 🔧 Advanced Configuration

### Download State File
The application maintains `download_state.json` for:
- Tracking download progress
- Resuming interrupted downloads
- Storing file size information

### Size Tolerance Explained
- `0.8`: File must be at least 80% of expected size to be considered complete
- `0.9`: File must be at least 90% of expected size (recommended)
- `0.95`: File must be at least 95% of expected size (strict)
- `0.99`: File must be at least 99% of expected size (very strict)

## 🐛 Troubleshooting

### Common Issues

1. **"Module not found" error**
   ```bash
   pip install yt-dlp
   ```

2. **Network timeouts**
   - Increase retry attempts: Use `--retries` option
   - Check internet connection
   - Try different resolution

3. **Download stuck at 0%**
   - Video might be unavailable in your region
   - Try different video/playlist
   - Check URL format

4. **File skipped incorrectly**
   - Adjust `--size-tolerance` to lower value
   - Manually delete incomplete files
   - Use "Check File Status" in GUI

### Performance Tips
- Use wired connection for large downloads
- Close bandwidth-intensive applications during download
- For playlists, start with lower resolution to test

## 📊 Sample Output

### CLI Output
```
🔍 Fetching video information...
🎬 Video: My Awesome Video
📏 Expected size: 156.3MB
💾 Resolution: best
📝 Subtitles: No
📊 File analysis:
   📁 Existing file: My_Awesome_Video.mp4
   💾 File size: 152.1MB
   📏 Expected size: 156.3MB
   ✅ Completion: 97.3%
⏭️  SKIPPED: Video already downloaded completely
```

### GUI Log Output
```
[14:30:25] 🔍 Fetching video information...
[14:30:26] 🎬 Video: My Awesome Video
[14:30:26] 📥 Progress: 45.2% (67.1MB/148.3MB) | Speed: 1.2MiB/s | ETA: 01:15
[14:31:45] ✅ Download completed: My_Awesome_Video.mp4
```

## 🤝 Contributing

Contributions are welcome! Please feel free to submit pull requests or open issues for:
- Bug reports
- Feature requests
- Documentation improvements
- Code optimizations

## ⚠️ Disclaimer

This tool is intended for:
- Downloading content you have the right to access
- Personal archival purposes
- Educational use

Please respect:
- YouTube's Terms of Service
- Copyright laws
- Content creators' rights

The developers are not responsible for misuse of this software.

## 📄 License

This project is provided for educational and personal use. Please ensure you comply with all applicable laws and terms of service when using this software.

## 🆘 Support

If you encounter issues:
1. Check the troubleshooting section above
2. Ensure all dependencies are installed
3. Verify your YouTube URLs are correct
4. Check available disk space in output directory

For persistent issues, please open a GitHub issue with:
- Error messages
- Steps to reproduce
- Your operating system and Python version

---


**Happy Downloading!** 🎉
