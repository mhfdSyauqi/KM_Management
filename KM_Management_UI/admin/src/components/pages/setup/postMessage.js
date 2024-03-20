import { GetCategoryReferenceAsync } from '@/api/reqCategory.js'
import { ref } from 'vue'
import { PostMessageAsync } from '@/api/reqSetup.js'

const categoryRef = ref([])

const newMessage = ref({
  type: '',
  contents: '',
})

const errorPost = ref({
  contents: { isError: false, message: '' }
})

async function HandlePublish() {

  const postMessage = await PostMessageAsync(
    newMessage.value.type,
    newMessage.value.contents,
  )

  if (!postMessage.is_success) {
    if (postMessage.error.statuscode === 400) {
      errorPost.value.contents.isError = true
      errorPost.value.contents.message = postMessage.error.message
    }
    return postMessage.error.statuscode
  }

  ResetPostInput()
  return true
}

function ResetPostInput() {
  for (const key in newMessage.value) {
    newMessage.value[key] = ''
  }

  for (const key in errorPost.value) {
    errorPost.value[key].isError = false
    errorPost.value[key].message = ''
  }
}

export { newMessage, errorPost, HandlePublish, ResetPostInput }