import { defineStore } from 'pinia';


export const useCategoriesStore = defineStore({
  id: 'categoriesListStore',
  state: () => ({
    firstLayer: [],
    secondLayer: [],
    thirdLayer: [],
    selectedFirstUid: null,
    selectedSecondUid: null,
    selectedThirdUid: null,
    hightLightUid:null,

  }),
  getters: {

    getFirstUid: () => (name) => {
      return localStorage.getItem(`${name}`); 
    },
    getSecondUid: () => (name) => {
      return localStorage.getItem(`${name}`); 
    },
    getThirdUid: () => (name) => {
      return localStorage.getItem(`${name}`); 
    },
    getFirstActive: () => (name) => {
      return localStorage.getItem(`active${name}`);
    },
    getSecondActive: () => (name) => {
      return localStorage.getItem(`active${name}`);
    },
    getThirdActive: () => (name) => {
      return localStorage.getItem(`active${name}`);
    },
    getSearchFirst: (state) => (name) => {
      const categoryWords =  name.toLowerCase().split(' ');
  
      return state.firstLayer
          .filter(content => {
              return categoryWords.every(word => {
                  return content.name.toLowerCase().includes(word);
              });
          })
      },
      getSearchSecond: (state) => (name) => {
        const categoryWords =  name.toLowerCase().split(' ');

        return state.secondLayer
            .filter(content => {
                return categoryWords.every(word => {
                    return content.name.toLowerCase().includes(word);
                });
            })
      },
      getSearchThird: (state) => (name) => {
        const categoryWords =  name.toLowerCase().split(' ');

        return state.thirdLayer
            .filter(content => {
                return categoryWords.every(word => {
                    return content.name.toLowerCase().includes(word);
                });
            })
      },
    
   getHightlightUid(){
    return this.hightLightUid;
   },
 
  },
  actions: {
     
      setSelectedFirstUid(name, uid, is_Active) {
        localStorage.setItem(`${name}`, uid);
        localStorage.setItem(`active${name}`, is_Active);
        
      },
      setSelectedSecondUid(name,uid, is_Active) {
        localStorage.setItem(`${name}`, uid);
        localStorage.setItem(`active${name}`, is_Active);
      },
      setSelectedThirdUid(name, uid, is_Active) {
        localStorage.setItem(`${name}`, uid);
        localStorage.setItem(`active${name}`, is_Active);
      },
      setHightLight(uid) {
        this.hightLightUid = uid;
      },
      setSearchFirst(data){
        this.firstLayer = data;
      },
      setSearchSecond(data){
        this.secondLayer = data;
      },
      setSearchThird(data){
        this.thirdLayer = data;
      },
  },
});
