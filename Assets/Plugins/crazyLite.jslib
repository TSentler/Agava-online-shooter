mergeInto(LibraryManager.library, {

  InitSDKLite: function (version) {
    if (!window.Crazygames) {
      return false;
    }
    window.Crazygames.init({
      version: typeof UTF8ToString !== 'undefined' ? UTF8ToString(version) : Pointer_stringify(version), // Pointer_stringify is obsolete, use UTF8ToString where possible
      sdkType: "unity-lite",
    });
  }
});
