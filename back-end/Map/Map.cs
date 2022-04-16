namespace back_end.Map
{
     public class Map
    {
        async public Task start()
        {
            // map ApiTypeMap
            ApiTypeMap apiTypeMap = new ApiTypeMap();
            await apiTypeMap.start();

            // map callbackMap
            CallbackMap callbackMap = new CallbackMap();
            await callbackMap.start();

            // map nameComponentsFormMap
            NameComponentsFormMap nameComponentsFormMap = new NameComponentsFormMap();
            await nameComponentsFormMap.start();

            // map nameComponentsMap
            NameComponentsMap nameComponentsMap = new NameComponentsMap();
            await nameComponentsMap.start();
        }
    }
}
