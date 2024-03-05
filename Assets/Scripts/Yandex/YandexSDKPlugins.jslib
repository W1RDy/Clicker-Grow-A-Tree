mergeInto(LibraryManager.library, {

  // AuthorizeExtern: function (){
  //   auth();
  // },

  ShowAdv: function(){
    ysdk.adv.showFullscreenAdv({
      callbacks: {
        onClose: function(wasShown) {
          myGameInstance.SendMessage('BrowserRequestHandler', 'ContinueMusic');
          myGameInstance.SendMessage('BrowserRequestHandler', 'StartShowingADV')
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
          myGameInstance.SendMessage('BrowserRequestHandler', 'GetReward', value)
          isRewarded = true;
        },
        onClose: function(){
          console.log('Video ad closed.');
          myGameInstance.SendMessage('BrowserRequestHandler', 'ContinueMusic');
          }
        }, 
        onError: function(e){
          console.log('Error while open video ad:', e);
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

  PlayerIsInitializedExtern: function()
  {
    var isInitialized = String(player != undefined);
    var bufferSize = lengthBytesUTF8(isInitialized) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(isInitialized, buffer, bufferSize);
    return buffer;
  },

  SaveDataExtern: function (data){
    console.log("SaveExtern")
    var dataString = UTF8ToString(data);
    var myObj = JSON.parse(dataString); 
    player.setData(myObj);
  },

  LoadDataExtern: function (){
      console.log('Load data');
      console.log(player);
      console.log(lb)
      player.getData().then(function(_data ){
            const myJSON = JSON.stringify(_data);
            myGameInstance.SendMessage('SaveService', 'SetData', myJSON);
      });
  },

});
