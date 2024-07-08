import { ref } from 'vue'
import { GetSetupGeneralAsync, PatchSetupGeneralAsync } from '@/api/reqSetupGeneral.js'
import { ConfirmSwal, ToastSwal } from '@/extension/SwalExt.js'

const generalModel = ref({
  category: {
    layer_one_limit: null,
    layer_two_limit: null,
    layer_three_limit: null,
    suggestion_limit: null
  },
  timing: {
    delay_typing: null,
    idle_duration: null
  },
  mailing: {
    mail_history_from: null,
    mail_history_subject: null
  },
  helpdesk: {
    mail_helpdesk_from: null,
    mail_helpdesk_to: null,
    mail_helpdesk_subject: null,
    mail_helpdesk_content: null,
    mail_helpdesk_content_old: null,
    mail_helpdesk_content_html: null
  },
  others: {
    keywords: null
  },
  mailbot: {
    email: null,
    password: null,
    server: null,
    port: null
  }
})

const generalError = ref({
  category: {
    layer_one_limit: { isError: false, message: '' },
    layer_two_limit: { isError: false, message: '' },
    layer_three_limit: { isError: false, message: '' },
    suggestion_limit: { isError: false, message: '' }
  },
  timing: {
    delay_typing: { isError: false, message: '' },
    idle_duration: { isError: false, message: '' }
  },
  mailing: {
    mail_history_from: { isError: false, message: '' },
    mail_history_subject: { isError: false, message: '' }
  },
  helpdesk: {
    mail_helpdesk_from: { isError: false, message: '' },
    mail_helpdesk_to: { isError: false, message: '' },
    mail_helpdesk_subject: { isError: false, message: '' },
    mail_helpdesk_content: { isError: false, message: '' }
  },
  others: {
    keywords: { isError: false, message: '' }
  },
  mailbot: {
    email: { isError: false, message: '' },
    password: { isError: false, message: '' },
    server: { isError: false, message: '' },
    port: { isError: false, message: '' }
  }
})

async function GetSetupGeneral() {
  const result = await GetSetupGeneralAsync()

  if (result.is_success) {
    generalModel.value = result.general
  }
}

async function HandleCategoriesLimit() {
  const minLimit = 3
  const maxLimit = 12

  for (const key in generalModel.value.category) {
    const keyValue = generalModel.value.category[key]

    if (keyValue < minLimit) {
      generalError.value.category[key].isError = true
      generalError.value.category[key].message = 'minimal value was 3'
    } else if (keyValue > maxLimit) {
      generalError.value.category[key].isError = true
      generalError.value.category[key].message = 'maximal value was 12'
    } else {
      generalError.value.category[key].isError = false
      generalError.value.category[key].message = ''
    }
  }
}

async function HandleTypingLimit() {
  const minLimit = 0.5
  const maxLimit = 1.5

  const keyValue = generalModel.value.timing.delay_typing

  if (keyValue < minLimit) {
    console.log(keyValue)
    generalError.value.timing.delay_typing.isError = true
    generalError.value.timing.delay_typing.message = 'minimal value was 0.5'
  } else if (keyValue > maxLimit) {
    generalError.value.timing.delay_typing.isError = true
    generalError.value.timing.delay_typing.message = 'maximal value was 1.5'
  } else {
    generalError.value.timing.delay_typing.isError = false
    generalError.value.timing.delay_typing.message = ''
  }
}

async function HandleIdleLimit() {
  const minLimit = 5
  const maxLimit = 30

  const keyValue = generalModel.value.timing.idle_duration

  if (keyValue < minLimit) {
    generalError.value.timing.idle_duration.isError = true
    generalError.value.timing.idle_duration.message = 'minimal value was 5'
  } else if (keyValue > maxLimit) {
    generalError.value.timing.idle_duration.isError = true
    generalError.value.timing.idle_duration.message = 'maximal value was 30'
  } else {
    generalError.value.timing.idle_duration.isError = false
    generalError.value.timing.idle_duration.message = ''
  }
}

async function HandleOthersLimit() {
  const minLimit = 3
  const keyValue = generalModel.value.others.keywords

  if (keyValue < minLimit) {
    generalError.value.others.keywords.isError = true
    generalError.value.others.keywords.message = 'minimal value was 3'
  } else {
    generalError.value.others.keywords.isError = false
    generalError.value.others.keywords.message = ''
  }
}

async function HandleEmptyMailing() {
  for (const key in generalModel.value.mailing) {
    const keyValue = generalModel.value.mailing[key]
    if (keyValue === '') {
      generalError.value.mailing[key].isError = true
      generalError.value.mailing[key].message = 'required'
    } else {
      generalError.value.mailing[key].isError = false
      generalError.value.mailing[key].message = ''
    }
  }
}

async function HandleEmptyHelpdesk() {
  for (const key in generalModel.value.helpdesk) {
    const keyValue = generalModel.value.helpdesk[key]
    if (keyValue === '') {
      generalError.value.helpdesk[key].isError = true
      generalError.value.helpdesk[key].message = 'required'
    } else {
      generalError.value.helpdesk[key].isError = false
      generalError.value.helpdesk[key].message = ''
    }
  }
}

async function HandleMailBotEmpty() {
  for (const key in generalModel.value.mailbot) {
    const keyValue = generalModel.value.mailbot[key]
    if (keyValue === '' || keyValue === null) {
      generalError.value.mailbot[key].isError = true
      generalError.value.mailbot[key].message = 'required'
    } else {
      generalError.value.mailbot[key].isError = false
      generalError.value.mailbot[key].message = ''
    }
  }

  const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
  const validEmail = regex.test(generalModel.value.mailbot.email)
  if (!validEmail) {
    generalError.value.mailbot.email.isError = true
    generalError.value.mailbot.email.message = 'email address invalid'
  } else {
    generalError.value.mailbot.email.isError = false
    generalError.value.mailbot.email.message = ''
  }
}

async function HandleSave() {
  const isValid = ValidateEmptyOrError()
  if (!isValid) {
    return await ToastSwal.fire({ text: 'Please recheck your input', icon: 'error' })
  }

  ResetError()

  const { isConfirmed } = await ConfirmSwal.fire({ text: `are you sure with the changes?` })
  if (!isConfirmed) return

  const modelPayload = {
    Category: {
      ...generalModel.value.category
    },
    Timing: {
      delay_typing: generalModel.value.timing.delay_typing * 1000,
      idle_duration: generalModel.value.timing.idle_duration * 1000 * 60
    },
    Mailing: {
      ...generalModel.value.mailing
    },
    Helpdesk: {
      ...generalModel.value.helpdesk,
      mail_helpdesk_content:
        typeof generalModel.value.helpdesk.mail_helpdesk_content === 'string'
          ? generalModel.value.helpdesk.mail_helpdesk_content
          : JSON.stringify(generalModel.value.helpdesk.mail_helpdesk_content),
      mail_helpdesk_content_html: generalModel.value.helpdesk.mail_helpdesk_content_html
    },
    Others: {
      ...generalModel.value.others
    },
    Mailbot: {
      ...generalModel.value.mailbot
    }
  }

  const patchGeneral = await PatchSetupGeneralAsync(modelPayload)
  if (!patchGeneral.is_success) {
    return await ToastSwal.fire({
      text: 'Something went wrong!, please resubmit in few minute',
      icon: 'error'
    })
  }

  return await ToastSwal.fire({
    text: 'success modify general setup',
    icon: 'success'
  })
}

function ValidateEmptyOrError() {
  let isValid = true
  for (const key in generalError.value) {
    for (const errorKey in generalError.value[key]) {
      const isErr = generalError.value[key][errorKey].isError
      if (isErr) {
        isValid = false
      }
    }
  }

  if (generalModel.value.helpdesk.mail_helpdesk_content === '') {
    generalError.value.helpdesk.mail_helpdesk_content.isError = true
    generalError.value.helpdesk.mail_helpdesk_content.message = 'required'
    isValid = false
  }

  return isValid
}

function ResetError() {
  for (const error in generalError.value) {
    for (const errorKey in generalError.value[error]) {
      generalError.value[error][errorKey].isError = false
      generalError.value[error][errorKey].message = ''
    }
  }
}

export {
  generalModel,
  generalError,
  GetSetupGeneral,
  HandleCategoriesLimit,
  HandleTypingLimit,
  HandleIdleLimit,
  HandleOthersLimit,
  HandleEmptyMailing,
  HandleEmptyHelpdesk,
  HandleMailBotEmpty,
  HandleSave
}