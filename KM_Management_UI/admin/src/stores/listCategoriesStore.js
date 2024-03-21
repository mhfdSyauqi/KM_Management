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
    // errorMessageAddCategory:null,
    // errorMessageUpdateCategory:null,
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
    // getFirstLayer() {
    //   return this.firstLayer;
    // },
    // getSecondLayer() {
    //     return this.secondLayer;
    // },
    // getThirdLayer() {
    // return this.thirdLayer;
    // },
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
  //  getErrorMessageUpdateCategory(){
  //   return this.errorMessageUpdateCategory;
  //  },
  //  getErrorMessageAddCategory(){
  //   return this.errorMessageAddCategory;
  //  }
    
  },
  actions: {
    // async fetchFirstLayer(formData) {
    //     try {
    //       const response = await axios.post('https://localhost:7077/API/categorieslist/GetCategoriesByUidReference',formData, {
            
    //       });
      
    //       this.firstLayer = response.data;
    //     } catch (error) {
    //       console.error('Terjadi kesalahan:', error);
    //     }
    //   }, 
    //   async fetchSecondLayer(formData) {
    //     try {
      
    //       const response = await axios.post('https://localhost:7077/API/categorieslist/GetCategoriesByUidReference',formData, {
    //       });
      
    //       this.secondLayer = response.data;
    //     } catch (error) {
    //       console.error('Terjadi kesalahan:', error);
    //     }
    //   },
    //   async fetchThirdLayer(formData) {
    //     try {
      
    //       const response = await axios.post('https://localhost:7077/API/categorieslist/GetCategoriesByUidReference',formData ,{
    //       });
      
    //       this.thirdLayer = response.data;
    //     } catch (error) {
    //       console.error('Terjadi kesalahan:', error);
    //     }
    //   },
    //   async addNewCategory(newCategory) {
    //     try {
    //       this.errorMessageAddCategory = null;
    //       const result = await Swal.fire({
    //         icon: 'question',
    //         title: 'Are you sure?',
    //         showCancelButton: true,
    //         confirmButtonText: 'Yes',
    //         cancelButtonText: 'No',
    //         confirmButtonColor: '#f6993f',
    //         cancelButtonColor: '#d33',
    //       });
      
    //       if (result.isConfirmed) {
    //         const response = await axios.post('https://localhost:7077/API/categorieslist/AddNewCategory', newCategory);
      
    //         Swal.fire({
    //           icon: 'success',
    //           title: 'Add new category successful!',
    //           confirmButtonColor: '#f6993f',
    //         });
    //       } else if (result.dismiss === Swal.DismissReason.cancel) {
    //         Swal.fire({
    //           icon: 'info',
    //           title: 'Action canceled',
    //           confirmButtonColor: '#f6993f',
    //         });
    //       }
    //     } catch (error) {
    //       console.log(error.response.data.errors.Name);
    //       if (error.response.data.errors.Name) {
    //         this.errorMessageAddCategory = error.response.data.errors.Name;
    //       } else {
    //         Swal.fire({
    //           title: 'Error',
    //           text: 'Failed to add a new categoryy',
    //           icon: 'error',
    //           confirmButtonColor: '#d33',
    //         });
    //       }
    //     }
    //   },
    //   async updateCategory(updateCategory) {
    //     try {
    //       this.errorMessageUpdateCategory=null;
    //       const result = await Swal.fire({
    //         icon: 'question',
    //         title: 'Are you sure?',
    //         showCancelButton: true,
    //         confirmButtonText: 'Yes',
    //         cancelButtonText: 'No',
    //         confirmButtonColor: '#f6993f',
    //         cancelButtonColor: '#d33',
    //       });
      
    //       if (result.isConfirmed) {
    //         const response = await axios.put('https://localhost:7077/API/categorieslist/UpdateCategoriesList', updateCategory);
      
    //         Swal.fire({
    //           icon: 'success',
    //           title: 'Update category successful!',
    //           confirmButtonColor: '#f6993f',
    //         });
    //       } else if (result.dismiss === Swal.DismissReason.cancel) {
    //         Swal.fire({
    //           icon: 'info',
    //           title: 'Action canceled',
    //           confirmButtonColor: '#f6993f',
    //         });
    //       }
    //     } catch (error) {
    //       console.log(error);
    //       if (error.response.data.errors.Name) {
    //         this.errorMessageUpdateCategory = error.response.data.errors.Name;
    //       } else {
    //         Swal.fire({
    //           title: 'Error',
    //           text: 'Failed to update a category',
    //           icon: 'error',
    //           confirmButtonColor: '#d33',
    //         });
    //       }
    //     }
    //   },
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
