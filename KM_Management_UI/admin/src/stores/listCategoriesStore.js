import axios from 'axios';
import { defineStore } from 'pinia';
import Swal from 'sweetalert2';

export const useCategoriesStore = defineStore({
  id: 'categoriesListStore',
  state: () => ({
    firstLayer: [],
    secondLayer: [],
    thirdLayer: [],
    allDataLayer:[],
    selectedFirstUid: null,
    selectedSecondUid: null,
    selectedThirdUid: null,
    hightLightUid:null,

  }),
  getters: {
    getFirstUid: (state) => (name) => {
      return localStorage.getItem(`${name}`); 
    },
    getSecondUid: (state) => (name) => {
      return localStorage.getItem(`${name}`); 
    },
    getThirdUid: (state) => (name) => {
      return localStorage.getItem(`${name}`); 
    },
    getFirstActive: (state) => (name) => {
      return localStorage.getItem(`active${name}`);
    },
    getSecondActive: (state) => (name) => {
      return localStorage.getItem(`active${name}`);
    },
    getThirdActive: (state) => (name) => {
      return localStorage.getItem(`active${name}`);
    },
   
    getAllLayer(){
      return this.allDataLayer;
    },
    getSearch: (state) => (name) => {
      const categoryWords =  name.toLowerCase().split(' ');
  
      return state.allDataLayer
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
    
      async allLayer(alldata) {
        try {
      
          this.allDataLayer = alldata;
        } catch (error) {
          console.error('Terjadi kesalahan:', error);
        }
      },
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
  },
});
