import { defineStore } from "pinia";

export const useStore = defineStore({
  id: "topIssueStore",
  state: () => ({
    availableItems: [],
    selectedItems: [],
  }),
  getters: {
    getAvailableItems() {
      return this.availableItems;
    },
    getSelectedItems() {
      return this.selectedItems;
    },
    searchAvailableItems: (state) => (name) => {
      const categoryWords = name.toLowerCase().split(" ");

      return state.availableItems.filter((content) => {
        return categoryWords.every((word) => {
          return content.name.toLowerCase().includes(word);
        });
      });
    },
    searchSelectedItems: (state) => (name) => {
      const categoryWords = name.toLowerCase().split(" ");

      return state.selectedItems.filter((content) => {
        return categoryWords.every((word) => {
          return content.name.toLowerCase().includes(word);
        });
      });
    },
  },
  actions: {
    setSelectedItems(selectedItem) {
        this.selectedItems = selectedItem;
    },
    setAvailableItems(availableItem) {
        this.availableItems = availableItem;
    },
  },
});
