mergeInto(LibraryManager.library, {

  // AuthorizeExtern: function (){
  //   auth();
  // },

  ShowAdv: function(){
    ysdk.adv.showFullscreenAdv({
      callbacks: {
        onClose: function(wasShown) {
          myGameInstance.SendMessage('AudioPlayer', 'SetSettings', 'true');
        },
        onError: function(error) {
          // some action on error
        }
      }
    })
  },

  AddRewardForADV: function(value){
    var isRewarded = false;
    ysdk.adv.showRewardedVideo({
      callbacks: {
        onOpen: function(){
          console.log('Video ad open.');
        },
        onRewarded: function(){
          console.log('Rewarded!');
          myGameInstance.SendMessage('ButtonService', 'UpgradeCoinsCosts', value)
          isRewarded = true;
        },
        onClose: function(){
          console.log('Video ad closed.');
          myGameInstance.SendMessage('AudioManager', 'SetSettings', 'true');
          }
        }, 
        onError: function(e){
          console.log('Error while open video ad:', e);
        }
      }
    })
  },

  SaveScoreInLeaderboardExtern: function(score)
  {
    saveInLeaderboard(score);
  },

  GetLanguageExtern: function()
  {
    var lang = ysdk.environment.i18n.lang;
    var bufferSize = lengthBytesUTF8(lang) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(lang, buffer, bufferSize);
    return buffer;
  },

  PlayerIsInitialized: function()
  {
    var isInitialized = String(player != undefined);
    var bufferSize = lengthBytesUTF8(isInitialized) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(isInitialized, buffer, bufferSize);
    return buffer;
  },

});