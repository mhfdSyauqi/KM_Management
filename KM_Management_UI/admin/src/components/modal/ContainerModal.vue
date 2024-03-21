<template>
    <div
      class="fixed inset-0 z-50 flex items-center justify-center"
      @click="closeModalOutside"
    >
      <!-- Modal background -->
      <div class="fixed inset-0 bg-black opacity-50"></div>
  
      <!-- Modal content -->
      <div
        class="bg-white pl-8 pr-8 rounded-2xl z-50 w-auto min-w-auto min-h-auto"
        @click.stop
      >
        <slot></slot>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, defineEmits } from "vue";
  
  const emit = defineEmits(["close-modal"]);
  
  const closeModalOutside = (event) => {
    if (
      event.target.classList.contains("flex") ||
      event.target.classList.contains("bg-black")
    ) {
      emit("close-modal");
    }
  };
  </script>
  
  <style scoped></style>
  