<script setup>
import DynamicInput from '@/views/setup/message/DynamicInput.vue'
import { onMounted, ref } from 'vue'
import { active_message, GetActiveMessage } from '@/components/pages/setup/useMessage.js'

const length = 10
const expanded = ref(Array.from({ length }, () => false))

const openingMessage = ref([])
const isDropDownOpeningOpened = ref([])
const editModeOpening = ref([])
const updateTextOpening = ref([])

const layerOneMessage = ref([])
const isDropDownLayerOneOpened = ref([])
const editModeLayerOne = ref([])
const updateTextLayerOne = ref([])

const layerTwoMessage = ref([])
const isDropDownLayerTwoOpened = ref([])
const editModeLayerTwo = ref([])
const updateTextLayerTwo = ref([])

const layerThreeMessage = ref([])
const isDropDownLayerThreeOpened = ref([])
const editModeLayerThree = ref([])
const updateTextLayerThree = ref([])

const solvedMessage = ref([])
const isDropDownSolvedOpened = ref([])
const editModeSolved = ref([])
const updateTextSolved = ref([])

const notFoundMessage = ref([])
const isDropDownNotFoundOpened = ref([])
const editModeNotFound = ref([])
const updateTextNotFound = ref([])

const reAskedMessage = ref([])
const isDropDownReAskedOpened = ref([])
const editModeReAsked = ref([])
const updateTextReAsked = ref([])

const suggestionMessage = ref([])
const isDropDownSuggestionOpened = ref([])
const editModeSuggestion = ref([])
const updateTextSuggestion = ref([])

const idleMessage = ref([])
const isDropDownIdleOpened = ref([])
const editModeIdle = ref([])
const updateTextIdle = ref([])

const closingMessage = ref([])
const isDropDownClosingOpened = ref([])
const editModeClosing = ref([])
const updateTextClosing = ref([])

const feedbackMessage = ref([])
const isDropDownFeedbackOpened = ref([])
const editModeFeedback = ref([])
const updateTextFeedback = ref([])

const mailSendedMessage = ref([])
const isDropDownMailSendedOpened = ref([])
const editModeMailSended = ref([])
const updateTextMailSended = ref([])

onMounted(() => {
  fetchOpening()
  fetchLayerOne()
  fetchLayerTwo()
  fetchLayerThree()
  fetchSolved()
  fetchNotFound()
  fetchReasked()
  fetchSuggestion()
  fetchIdle()
  fetchClosing()
  fetchFeedBack()
  fetchMailSended()
})

const fetchOpening = async () => {
  await GetActiveMessage('WELCOME')
  openingMessage.value = active_message.value
  isDropDownOpeningOpened.value = Array(openingMessage.value.length).fill(false)
  editModeOpening.value = Array(openingMessage.value.length).fill(false)
  updateTextOpening.value = openingMessage.value.map((opening) => opening.contents)
}

const fetchLayerOne = async () => {
  await GetActiveMessage('LAYER_ONE')
  layerOneMessage.value = active_message.value
  isDropDownLayerOneOpened.value = Array(layerOneMessage.value.length).fill(false)
  editModeLayerOne.value = Array(layerOneMessage.value.length).fill(false)
  updateTextLayerOne.value = layerOneMessage.value.map((layerOne) => layerOne.contents)
}

const fetchLayerTwo = async () => {
  await GetActiveMessage('LAYER_TWO')
  layerTwoMessage.value = active_message.value
  isDropDownLayerTwoOpened.value = Array(layerTwoMessage.value.length).fill(false)
  editModeLayerTwo.value = Array(layerTwoMessage.value.length).fill(false)
  updateTextLayerTwo.value = layerTwoMessage.value.map((layerTwo) => layerTwo.contents)
}

const fetchLayerThree = async () => {
  await GetActiveMessage('LAYER_THREE')
  layerThreeMessage.value = active_message.value
  isDropDownLayerThreeOpened.value = Array(layerThreeMessage.value.length).fill(false)
  editModeLayerThree.value = Array(layerThreeMessage.value.length).fill(false)
  updateTextLayerThree.value = layerThreeMessage.value.map((layerThree) => layerThree.contents)
}

const fetchSolved = async () => {
  await GetActiveMessage('SOLVED')
  solvedMessage.value = active_message.value
  isDropDownSolvedOpened.value = Array(solvedMessage.value.length).fill(false)
  editModeSolved.value = Array(solvedMessage.value.length).fill(false)
  updateTextSolved.value = solvedMessage.value.map((solved) => solved.contents)
}

const fetchNotFound = async () => {
  await GetActiveMessage('NOT_FOUND')
  notFoundMessage.value = active_message.value
  isDropDownNotFoundOpened.value = Array(notFoundMessage.value.length).fill(false)
  editModeNotFound.value = Array(notFoundMessage.value.length).fill(false)
  updateTextNotFound.value = notFoundMessage.value.map((notFound) => notFound.contents)
}

const fetchReasked = async () => {
  await GetActiveMessage('REASKED')
  reAskedMessage.value = active_message.value
  isDropDownReAskedOpened.value = Array(reAskedMessage.value.length).fill(false)
  editModeReAsked.value = Array(reAskedMessage.value.length).fill(false)
  updateTextReAsked.value = reAskedMessage.value.map((reAsked) => reAsked.contents)
}

const fetchSuggestion = async () => {
  await GetActiveMessage('SUGGESTION')
  suggestionMessage.value = active_message.value
  isDropDownSuggestionOpened.value = Array(suggestionMessage.value.length).fill(false)
  editModeSuggestion.value = Array(suggestionMessage.value.length).fill(false)
  updateTextSuggestion.value = suggestionMessage.value.map((suggestion) => suggestion.contents)
}

const fetchIdle = async () => {
  await GetActiveMessage('IDLE')
  idleMessage.value = active_message.value
  isDropDownIdleOpened.value = Array(idleMessage.value.length).fill(false)
  editModeIdle.value = Array(idleMessage.value.length).fill(false)
  updateTextIdle.value = idleMessage.value.map((idle) => idle.contents)
}

const fetchClosing = async () => {
  await GetActiveMessage('CLOSING')
  closingMessage.value = active_message.value
  isDropDownClosingOpened.value = Array(closingMessage.value.length).fill(false)
  editModeClosing.value = Array(closingMessage.value.length).fill(false)
  updateTextClosing.value = closingMessage.value.map((closing) => closing.contents)
}

const fetchFeedBack = async () => {
  await GetActiveMessage('FEEDBACK')
  feedbackMessage.value = active_message.value
  isDropDownFeedbackOpened.value = Array(feedbackMessage.value.length).fill(false)
  editModeFeedback.value = Array(feedbackMessage.value.length).fill(false)
  updateTextFeedback.value = feedbackMessage.value.map((feedback) => feedback.contents)
}

const fetchMailSended = async () => {
  await GetActiveMessage('MAIL_SENDED')
  mailSendedMessage.value = active_message.value
  isDropDownMailSendedOpened.value = Array(mailSendedMessage.value.length).fill(false)
  editModeMailSended.value = Array(mailSendedMessage.value.length).fill(false)
  updateTextMailSended.value = mailSendedMessage.value.map((mailSended) => mailSended.contents)
}

const toggleExpand = (index) => {
  expanded.value.forEach((value, idx) => {
    if (idx !== index) {
      expanded.value[idx] = false
    }
  })

  expanded.value[index] = !expanded.value[index]
}
</script>

<template>
  <div class="flex items-center align-middle gap-3 mb-3">
    <p class="text-sm text-[#999999]">Setup</p>

    <span> > </span>
    <p class="text-sm text-orange-400">Message</p>
  </div>

  <div
    class="w-full rounded-lg bg-white min-h-[25%] max-h-[96%] overflow-auto box-border flex flex-col"
  >
    <div class="flex gap-2.5 items-center py-5 px-8">
      <div class="basis-[84%] flex gap-3 items-center">
        <div class="w-full">
          <div class="flex items-start space-x-2 pb-8">
            <h1 class="text-2xl font-bold text-green-800">Messages</h1>
          </div>
          <div class="bg-[#fff3e5] w-fit p-3.5 text-sm xl:text-base rounded-tr-3xl">
            <p>
              Use <span class="font-medium text-[#e99300]">@username </span>
              <span class="ml-0.5">➡️</span> value of Username in conversation
            </p>
            <p>
              Use <span class="font-medium text-[#e99300]">@category </span>
              <span class="ml-2">➡️</span> value of Category Response in conversation
            </p>
          </div>
        </div>
      </div>
    </div>

    <div class="flex flex-col">
      <div class="pl-10 pr-10 pb-10 pt-5">
        <dynamicInput
          :nameInput="'Opening'"
          :messageData="openingMessage"
          :updateText="updateTextOpening"
          :editMode="editModeOpening"
          :typeMessage="'WELCOME'"
          :isDropDownOpen="isDropDownOpeningOpened"
          :fetchData="fetchOpening"
          :expanded="expanded[0]"
          @toggleExpand="toggleExpand(0)"
        />

        <dynamicInput
          :nameInput="'Choose Main Menu'"
          :messageData="layerOneMessage"
          :updateText="updateTextLayerOne"
          :editMode="editModeLayerOne"
          :typeMessage="'LAYER_ONE'"
          :isDropDownOpen="isDropDownLayerOneOpened"
          :fetchData="fetchLayerOne"
          :expanded="expanded[1]"
          @toggleExpand="toggleExpand(1)"
        />

        <dynamicInput
          :nameInput="'Choose Sub Menu - Layer 2'"
          :messageData="layerTwoMessage"
          :updateText="updateTextLayerTwo"
          :editMode="editModeLayerTwo"
          :typeMessage="'LAYER_TWO'"
          :isDropDownOpen="isDropDownLayerTwoOpened"
          :fetchData="fetchLayerTwo"
          :expanded="expanded[2]"
          @toggleExpand="toggleExpand(2)"
        />

        <dynamicInput
          :nameInput="'Choose Sub Menu - Layer 3'"
          :messageData="layerThreeMessage"
          :updateText="updateTextLayerThree"
          :editMode="editModeLayerThree"
          :typeMessage="'LAYER_THREE'"
          :isDropDownOpen="isDropDownLayerThreeOpened"
          :fetchData="fetchLayerThree"
          :expanded="expanded[3]"
          @toggleExpand="toggleExpand(3)"
        />

        <dynamicInput
          :nameInput="'After Solved'"
          :messageData="solvedMessage"
          :updateText="updateTextSolved"
          :editMode="editModeSolved"
          :typeMessage="'SOLVED'"
          :isDropDownOpen="isDropDownSolvedOpened"
          :fetchData="fetchSolved"
          :expanded="expanded[4]"
          @toggleExpand="toggleExpand(4)"
        />

        <dynamicInput
          :nameInput="'Not Found'"
          :messageData="notFoundMessage"
          :updateText="updateTextNotFound"
          :editMode="editModeNotFound"
          :typeMessage="'NOT_FOUND'"
          :isDropDownOpen="isDropDownNotFoundOpened"
          :fetchData="fetchNotFound"
          :expanded="expanded[5]"
          @toggleExpand="toggleExpand(5)"
        />

        <dynamicInput
          :nameInput="'Ever Asked'"
          :messageData="reAskedMessage"
          :updateText="updateTextReAsked"
          :editMode="editModeReAsked"
          :typeMessage="'REASKED'"
          :isDropDownOpen="isDropDownReAskedOpened"
          :fetchData="fetchReasked"
          :expanded="expanded[6]"
          @toggleExpand="toggleExpand(6)"
        />

        <dynamicInput
          :nameInput="'Suggestion'"
          :messageData="suggestionMessage"
          :updateText="updateTextSuggestion"
          :editMode="editModeSuggestion"
          :typeMessage="'SUGGESTION'"
          :isDropDownOpen="isDropDownSuggestionOpened"
          :fetchData="fetchSuggestion"
          :expanded="expanded[7]"
          @toggleExpand="toggleExpand(7)"
        />

        <dynamicInput
          :nameInput="'Idle'"
          :messageData="idleMessage"
          :updateText="updateTextIdle"
          :editMode="editModeIdle"
          :typeMessage="'IDLE'"
          :isDropDownOpen="isDropDownIdleOpened"
          :fetchData="fetchIdle"
          :expanded="expanded[8]"
          @toggleExpand="toggleExpand(8)"
        />

        <dynamicInput
          :nameInput="'Closing'"
          :messageData="closingMessage"
          :updateText="updateTextClosing"
          :editMode="editModeClosing"
          :typeMessage="'CLOSING'"
          :isDropDownOpen="isDropDownClosingOpened"
          :fetchData="fetchClosing"
          :expanded="expanded[9]"
          @toggleExpand="toggleExpand(9)"
        />

        <dynamicInput
          :nameInput="'Closing After Given Feedback'"
          :messageData="feedbackMessage"
          :updateText="updateTextFeedback"
          :editMode="editModeFeedback"
          :typeMessage="'FEEDBACK'"
          :isDropDownOpen="isDropDownFeedbackOpened"
          :fetchData="fetchFeedBack"
          :expanded="expanded[10]"
          @toggleExpand="toggleExpand(10)"
        />

        <dynamicInput
          :nameInput="'Closing After Send Mail'"
          :messageData="mailSendedMessage"
          :updateText="updateTextMailSended"
          :editMode="editModeMailSended"
          :typeMessage="'MAIL_SENDED'"
          :isDropDownOpen="isDropDownMailSendedOpened"
          :fetchData="fetchMailSended"
          :expanded="expanded[11]"
          @toggleExpand="toggleExpand(11)"
        />
      </div>
    </div>
  </div>
</template>

<style scoped>
::-webkit-scrollbar {
  width: 0.15rem;
}

/* Track */
::-webkit-scrollbar-track {
  box-shadow: inset 0 0 5px grey;
}

/* Handle */
::-webkit-scrollbar-thumb {
  background: rgba(94, 109, 92, 0.6);
}

/* Handle on hover */
::-webkit-scrollbar-thumb:hover {
  background: rgba(94, 109, 92, 1);
}
</style>