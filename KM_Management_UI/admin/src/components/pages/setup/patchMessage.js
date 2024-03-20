import { ref } from 'vue'
import { PatchMessageAsync } from '@/api/reqSetup.js'

const editMessage = ref({
    type: '',
    sequence: '',
    contents: '',
    is_active: '',
})

const errorEdit = ref({
  contents: { isError: false, message: '' },
})



async function HandleRePublish() {

  const patchMessage = await PatchMessageAsync(
    editMessage.value.type,
    editMessage.value.sequence,
    editMessage.value.contents,
    editMessage.value.is_active,
  )

  if (!patchMessage.is_success) {
    if (patchMessage.error.statuscode === 400) {
      errorEdit.value.contents.isError = true
      errorEdit.value.contents.message = patchMessage.error.message
    }
    return patchMessage.error.statuscode
  }

ResetEditInput()
  return true
}

function ResetEditInput() {
  for (const key in editMessage.value) {
    editMessage.value[key] = ''
  }

  for (const key in errorEdit.value) {
    errorEdit.value[key].isError = false
    errorEdit.value[key].message = ''
  }
}

export {
  editMessage,
  errorEdit,
  HandleRePublish,
  ResetEditInput
}