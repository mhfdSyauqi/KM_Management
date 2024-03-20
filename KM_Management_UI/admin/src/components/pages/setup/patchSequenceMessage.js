import { ref } from 'vue'
import { PatchSequenceMessageAsync } from '@/api/reqSetup.js'

const editSequence = ref({
    type: '',
    current_sequence: '',
    new_sequence:''
})

const errorSequence = ref({
  contents: { isError: false, message: '' },
})



async function HandleSequence() {

  const patchSequence = await PatchSequenceMessageAsync(
    editSequence.value.type,
    editSequence.value.current_sequence,
    editSequence.value.new_sequence,
  )

  if (!patchSequence.is_success) {
    if (patchSequence.error.statuscode === 400) {
      errorSequence.value.contents.isError = true
      errorSequence.value.contents.message = patchSequence.error.message
    }
    return patchSequence.error.statuscode
  }

  ResetSequence()
  return true
}

function ResetSequence() {
  for (const key in editSequence.value) {
    editSequence.value[key] = ''
  }

  for (const key in errorSequence.value) {
    errorSequence.value[key].isError = false
    errorSequence.value[key].message = ''
  }
}

export {
  editSequence,
  errorSequence,
  HandleSequence,
  ResetSequence
}