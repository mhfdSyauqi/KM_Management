import { ref } from 'vue'
import { DeleteMessageAsync } from '@/api/reqSetup.js'

const deleteMessage = ref({
    type: '',
    sequence: '',
})

const errorDelete = ref({
  contents: { isError: false, message: '' },
})



async function HandleUnPublish() {

  const patchMessage = await DeleteMessageAsync(
    deleteMessage.value.type,
    deleteMessage.value.sequence,
  )

  if (!patchMessage.is_success) {
    if (patchMessage.error.statuscode === 400) {
      errorDelete.value.contents.isError = true
      errorDelete.value.contents.message = patchMessage.error.message
    }
    return patchMessage.error.statuscode
  }

ResetDeleteInput()
  return true
}

function ResetDeleteInput() {
  for (const key in deleteMessage.value) {
    deleteMessage.value[key] = ''
  }

  for (const key in errorDelete.value) {
    errorDelete.value[key].isError = false
    errorDelete.value[key].message = ''
  }
}

export {
  deleteMessage,
  errorDelete,
  HandleUnPublish,
  ResetDeleteInput
}